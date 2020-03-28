<?php

namespace app\modules\v1\base;

use yii\data\ActiveDataProvider;
use yii\data\Pagination;
use yii\filters\auth\HttpBearerAuth;
use yii\filters\Cors;
use yii\helpers\ArrayHelper;
use yii\rest\ActiveController;
use yii\web\Response;

class BaseActiveController extends ActiveController
{
    public function behaviors()
    {
        $behaviors = parent::behaviors();
        $behaviors['contentNegotiator'] = [
            'class' => 'yii\filters\ContentNegotiator',
            'formats' => [
                'application/json' => Response::FORMAT_JSON,
                'text/html' => Response::FORMAT_HTML,
            ],
        ];
        $behaviors['authenticator'] = [
            'class' => HttpBearerAuth::class,
        ];
        $freshBehaviors = [
            'corsFilter' => [
                'class' => Cors::class,
                'cors' => [
                    'Origin' => ['*'],
                ],
            ],
        ];
        return ArrayHelper::merge($freshBehaviors, $behaviors);
    }

    /**
     * Set the status code in the response object
     * @param int $statusCode
     */
    public function statusCode($statusCode)
    {
        \Yii::$app->response->statusCode = $statusCode;
    }

    /**
     * Return the validation errors with a [400 - Bad Request] status code
     * @param array $errors
     * @return array
     */
    public function validationError($errors)
    {
        \Yii::$app->response->statusCode = 400;
        return $errors;
    }

    /**
     * @param \yii\db\ActiveQuery $helpsQuery
     * @return ActiveDataProvider
     */
    public function getActiveDataProvider(\yii\db\ActiveQuery $helpsQuery)
    {
        return new ActiveDataProvider([
            'query' => $helpsQuery,
            'pagination' => new Pagination([
                'validatePage' => false,
                'defaultPageSize' => 10,
            ]),
        ]);
    }
}
