<?php

namespace app\modules\v1\controllers;

use app\modules\v1\base\BaseController;

class AuthorizationController extends BaseController
{
    public function actionCreate()
    {
        $data = \Yii::$app->request->bodyParams;
        return $this->asJson($data);
    }
}
