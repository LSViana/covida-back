<?php

namespace app\modules\v1\controllers;

use app\modules\v1\base\BaseController;
use Yii;
use yii\helpers\Url;
use yii\web\ErrorAction;

/**
 * @SWG\Swagger(
 *     basePath="/v1/general/docs",
 *     produces={"application/json"},
 *     consumes={"application/json"},
 *     @SWG\Info(version="1.0.0", title="Covida API"),
 * )
 */
class GeneralController extends BaseController
{
    public function behaviors()
    {
        $behaviors = parent::behaviors();
        unset($behaviors['authenticator']);
        return $behaviors;
    }

    public function actions()
    {
        $actions = parent::actions();
        $actions['docs'] = [
            'class' => 'yii2mod\swagger\SwaggerUIRenderer',
            'restUrl' => Url::to(['general/json-schema']),
        ];
        $actions['json-schema'] = [
            'class' => 'yii2mod\swagger\OpenAPIRenderer',
            // Тhe list of directories that contains the swagger annotations.
            'scanDir' => [
                Yii::getAlias('@app/modules/v1/controllers'),
                Yii::getAlias('@app/modules/v1/models'),
            ],
        ];
        $actions['error'] = [
            'class' => ErrorAction::class,
        ];
        return $actions;
    }

    public function actionIndex()
    {
        return [
            'name' => Yii::$app->controller->module->params['name'],
            'version' => Yii::$app->controller->module->params['version'],
            'docs' => Url::to(['general/docs'], true),
        ];
    }
}
