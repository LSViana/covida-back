<?php

namespace app\modules\v1\models;

use Yii;

/**
 * This is the model class for table "help_item".
 *
 * @property int $id
 * @property string $name
 * @property int $amount
 * @property bool $complete
 * @property int $helpId
 *
 * @property Help $help
 */
class HelpItem extends \yii\db\ActiveRecord
{
    /**
     * {@inheritdoc}
     */
    public static function tableName()
    {
        return 'help_item';
    }

    /**
     * {@inheritdoc}
     */
    public function rules()
    {
        return [
            [['name', 'amount', 'complete', 'helpId'], 'required'],
            [['amount', 'helpId'], 'default', 'value' => null],
            [['helpId'], 'integer'],
            [['amount'], 'integer', 'max' => 100, 'min' => 1],
            [['complete'], 'boolean'],
            [['name'], 'string', 'max' => 64],
            [['helpId'], 'exist', 'skipOnError' => true, 'targetClass' => Help::className(), 'targetAttribute' => ['helpId' => 'id']],
        ];
    }

    /**
     * {@inheritdoc}
     */
    public function attributeLabels()
    {
        return [
            'id' => 'ID',
            'name' => 'Name',
            'amount' => 'Amount',
            'complete' => 'Complete',
            'helpId' => 'Help ID',
        ];
    }

    /**
     * Gets query for [[Help]].
     *
     * @return \yii\db\ActiveQuery
     */
    public function getHelp()
    {
        return $this->hasOne(Help::className(), ['id' => 'helpId']);
    }
}
