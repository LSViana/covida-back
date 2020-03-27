<?php

namespace app\modules\v1;

use yii\base\Module;

class ApiModule extends Module
{
    public function init()
    {
        parent::init();
        \Yii::configure($this, require __DIR__ . './config/module.php');
    }
}
