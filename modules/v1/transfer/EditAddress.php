<?php

namespace app\modules\v1\transfer;

use yii\base\Model;

class EditAddress extends Model
{
    public $lat;
    public $lng;
    public $address;

    public function rules()
    {
        return [
            [['lat', 'lng', 'address'], 'required'],
            [['lat', 'lng'], 'number'],
            [['address'], 'string', 'max' => 128],
        ];
    }
}
