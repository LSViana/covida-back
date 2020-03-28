<?php

namespace app\modules\v1\transfer;

use yii\base\Model;

class Login extends Model
{
    public $id;
    public $name;

    public function rules()
    {
        return [
            [['id', 'name'], 'required'],
            [['id'], 'number'],
            [['name'], 'string', 'max' => 64],
        ];
    }
}
