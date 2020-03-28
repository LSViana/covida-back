<?php

namespace app\modules\v1\transfer;

use app\modules\v1\definitions\ErrorMessages;
use app\modules\v1\models\HelpCategory;
use app\modules\v1\models\HelpItem;
use yii\base\Model;

class CreateHelp extends Model
{
    /** @var String[] */
    public $helpHasCategories;
    /** @var HelpItem[] */
    public $items;

    public function rules()
    {
        // // type_id needs to exist in the column "id" in the table defined in ProductType class
        //    ['type_id', 'exist', 'targetClass' => ProductType::class, 'targetAttribute' => ['type_id' => 'id']],
        return [
            [['helpHasCategories', 'items'], 'required'],
            [['helpHasCategories'], 'each', 'rule' => ['exist', 'targetClass' => HelpCategory::class, 'targetAttribute' => 'id']],
            [
                ['helpHasCategories'],
                function($attribute, $params, $validator) {
                    if(sizeof($this->helpHasCategories) === 0) {
                        $this->addError($attribute, ErrorMessages::ThisValueCantBeEmpty);
                    }
                },
            ],
        ];
    }
}
