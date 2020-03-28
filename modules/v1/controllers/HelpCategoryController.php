<?php

namespace app\modules\v1\controllers;

use app\modules\v1\base\BaseController;
use app\modules\v1\definitions\ErrorMessages;
use app\modules\v1\models\HelpCategory;
use app\modules\v1\models\User;
use yii\base\Model;

class HelpCategoryController extends BaseController
{
    public function actionIndex()
    {
        // Get the query
        $query = HelpCategory::find();
        // Return the paginated data using Yii ADP
        return $this->getActiveDataProvider($query);
    }

    public function actionView($id)
    {
        // Get the help category
        $helpCategory = HelpCategory::findOne(['id' => $id]);
        // If it exists
        if($helpCategory !== null)
        {
            // Return the help data
            return $helpCategory;
        }
        // Otherwise, return 400 - Bad Request
        $this->statusCode(400);
        $model = new Model();
        $model->addError('*', ErrorMessages::RecordNotFound);
        return $model->errors;
    }
}
