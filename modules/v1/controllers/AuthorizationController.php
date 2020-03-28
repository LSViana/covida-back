<?php

namespace app\modules\v1\controllers;

use app\modules\v1\base\BaseController;
use app\modules\v1\definitions\ErrorMessages;
use app\modules\v1\models\User;
use app\modules\v1\transfer\EditAddress;
use app\modules\v1\transfer\Login;
use yii\filters\auth\HttpBearerAuth;
use yii\filters\VerbFilter;
use yii\web\ServerErrorHttpException;

class AuthorizationController extends BaseController
{
    public function behaviors()
    {
        $behaviors = parent::behaviors();
        $behaviors['authenticator'] = [
            'class' => HttpBearerAuth::class,
            'except' => ['create', 'login']
        ];
        $behaviors['verbFilter'] = [
            'class' => VerbFilter::class,
            'actions' => [
                'edit-address' => ['PATCH'],
                'login' => ['POST'],
                'create' => ['POST'],
                'me' => ['GET']
            ],
        ];
        return $behaviors;
    }

    public function actionLogin()
    {
        // Get data from body
        $data = \Yii::$app->request->bodyParams;
        // Load and validate the data
        $login = new Login();
        $login->load($data, '');
        if($login->validate()) {
            // Find the user
            $user = User::findOne(['id' => $login->id]);
            // If user exists
            if($user !== null && $user->name === $login->name)
            {
                // Change scenario to login to output the accessToken
                $user->scenario = 'login';
                // Update access token
                $user->refreshAccessToken();
                // Save to database
                $user->save();
                // Return the user data as if it was me()
                return $user->toArray();
            }
            // Otherwise return 400 - Bad Request
            $this->statusCode(400);
            $login->addError('*', ErrorMessages::RecordNotFound);
            return $login->errors;
        }
        // Return the errors with 400
        $this->statusCode(400);
        return $login->errors;
    }

    public function actionCreate()
    {
        // Get data from body
        $data = \Yii::$app->request->bodyParams;
        $user = new User();
        // Load and validate the data
        $user->load($data, '');
        if($user->validate()) {
            // Create a new access token to the user
            $user->refreshAccessToken();
            // Save to database
            if($user->save()) {
                // Return value with 201 - Created
                $this->statusCode(201);
                return $user;
            } else {
                // Log error
                \Yii::$app->log->dispatch([
                    ErrorMessages::UnableToSaveRecord . " " . User::class
                ], false);
                // Return error to client
                $this->statusCode(500);
                $user->addError('*', ErrorMessages::UnableToSaveRecord);
                return $user->errors;
            }
        }
        // Return validation errors with 400 - Bad Request
        return $this->validationError($user->errors);
    }

    public function actionEditAddress()
    {
        // Get data from body
        $data = \Yii::$app->request->bodyParams;
        // Load and validate data
        $editAddress = new EditAddress();
        $editAddress->load($data, '');
        if($editAddress->validate()) {
            // Get user
            /** @var User $user */
            $user = \Yii::$app->user->getIdentity();
            // Set lat, lng and address to the user
            $user->load($editAddress->toArray(), '');
            // Save the user
            $user->save();
            // Return 204 - No content
            $this->statusCode(204);
            return;
        }
        // Return validation errors with 400 - Bad Request
        return $this->validationError($editAddress->errors);
    }

    public function actionMe()
    {
        // Get current user
        /** @var User $user */
        $user = \Yii::$app->user->getIdentity();
        // Return with 200 - OK
        return $user->toArray();
    }
}
