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
 * @property int $helpid
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
            [['name', 'amount', 'complete', 'helpid'], 'required'],
            [['amount', 'helpid'], 'default', 'value' => null],
            [['amount', 'helpid'], 'integer'],
            [['complete'], 'boolean'],
            [['name'], 'string', 'max' => 64],
            [['helpid'], 'exist', 'skipOnError' => true, 'targetClass' => Help::className(), 'targetAttribute' => ['helpid' => 'id']],
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
            'helpid' => 'Helpid',
        ];
    }

    /**
     * Gets query for [[Help]].
     *
     * @return \yii\db\ActiveQuery
     */
    public function getHelp()
    {
        return $this->hasOne(Help::className(), ['id' => 'helpid']);
    }
}
