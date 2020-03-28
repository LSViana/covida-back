<?php

namespace app\modules\v1\controllers;

use app\modules\v1\base\BaseController;
use app\modules\v1\definitions\ErrorMessages;
use app\modules\v1\models\Help;
use app\modules\v1\models\HelpCategory;
use app\modules\v1\models\HelpHasCategory;
use app\modules\v1\models\HelpItem;
use app\modules\v1\models\User;
use app\modules\v1\transfer\CreateHelp;
use PetstoreIO\Category;
use yii\base\Model;
use yii\data\ActiveDataProvider;
use yii\data\Pagination;
use yii\rest\ActiveController;
use yii\web\BadRequestHttpException;

class HelpController extends BaseController
{
    public function actionIndex()
    {
        // Create the query
        $helpsQuery = Help::find();
        // Return it with pagination using Yii ADP
        return $this->getActiveDataProvider($helpsQuery);
    }

    public function actionCreate()
    {
        // Get data from body
        $data = \Yii::$app->request->bodyParams;
        // Create help data transfer
        $helpCreate = new CreateHelp();
        $helpCreate->load($data, '');
        if($helpCreate->validate()) {
            // Create new help
            $help = new Help();
            $help->datetime = date('c');
            $help->status = 'awaiting';
            $help->userId = \Yii::$app->user->getIdentity()->id;
            // Save to database
            if($help->save()) {
                // Create helpHasCategories
                /** @var HelpHasCategory[] $helpHasCategories */
                $helpHasCategories = array_map(function($item) use ($help) {
                    $helpHasCategory = new HelpHasCategory();
                    $helpHasCategory->helpId = $help->id;
                    $helpHasCategory->helpCategoryId = $item;
                    if(!$helpHasCategory->validate()) {
                        throw new BadRequestHttpException(ErrorMessages::UnableToSaveRecord);
                    }
                    return $helpHasCategory;
                }, $helpCreate->helpHasCategories);
                // Create helpItems
                /** @var HelpItem[] $helpItems */
                $helpItems = array_map(function($item) use ($help) {
                    $helpItem = new HelpItem();
                    $helpItem->helpId = $help->id;
                    $helpItem->name = $item['name'];
                    $helpItem->amount = $item['amount'];
                    $helpItem->complete = false;
                    if(!$helpItem->validate()) {
                        throw new BadRequestHttpException(ErrorMessages::UnableToSaveRecord);
                    }
                    return $helpItem;
                }, $helpCreate->items);
                // Saving helpHasCategories and helpItems
                foreach($helpHasCategories as $item) {
                    $item->save();
                }
                foreach($helpItems as $item) {
                    $item->save();

                }
                // Return the help with status code 201
                $this->statusCode(201);
                $help = Help::find()
                    ->with('helpHasCategories.helpCategory')
                    ->with('helpItems')
                    ->with('user')
                    ->where(['id' => $help->id])
                    ->one();
                return $help->toArray([], [
                    'helpHasCategories',
                    'helpHasCategories.helpCategory',
                    'helpItems',
                    'user'
                ]);
            } else {
                // Log error
                \Yii::$app->log->dispatch([
                    ErrorMessages::UnableToSaveRecord . " " . Help::class
                ], false);
                $this->statusCode(500);
                $help->addError('*', ErrorMessages::UnableToSaveRecord);
                return $help->errors;
            }
        }
        // Return errors
        $this->statusCode(400);
        return $helpCreate->errors;
    }

    public function actionView($id)
    {
        // Get help with the given id
        $help = Help::find()
            ->with('helpHasCategories.helpCategory')
            ->with('helpItems')
            ->with('user')
            ->where(['id' => $id])
            ->one();
        // If it exists
        if($help !== null) {
            return $help->toArray([], [
                'helpHasCategories',
                'helpHasCategories.helpCategory',
                'helpItems',
                'user'
            ]);
        }
        // Return 400 - Bad requests
        $this->statusCode(400);
        $model = new Model();
        $model->addError('*', ErrorMessages::RecordNotFound);
        return $model->errors;
    }
}
