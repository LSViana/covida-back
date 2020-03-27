<?php

namespace app\modules\v1\models;

use Yii;

/**
 * This is the model class for table "help".
 *
 * @property int $id
 * @property string $datetime
 * @property bool $cancelleddatetime
 * @property string $cancelledreason
 * @property string $status
 * @property int $userid
 *
 * @property User $user
 * @property HelpItem[] $helpItems
 * @property Message[] $messages
 */
class Help extends \yii\db\ActiveRecord
{
    /**
     * {@inheritdoc}
     */
    public static function tableName()
    {
        return 'help';
    }

    /**
     * {@inheritdoc}
     */
    public function rules()
    {
        return [
            [['datetime', 'cancelleddatetime', 'cancelledreason', 'status', 'userid'], 'required'],
            [['datetime'], 'safe'],
            [['cancelleddatetime'], 'boolean'],
            [['status'], 'string'],
            [['userid'], 'default', 'value' => null],
            [['userid'], 'integer'],
            [['cancelledreason'], 'string', 'max' => 64],
            [['userid'], 'exist', 'skipOnError' => true, 'targetClass' => User::className(), 'targetAttribute' => ['userid' => 'id']],
        ];
    }

    /**
     * {@inheritdoc}
     */
    public function attributeLabels()
    {
        return [
            'id' => 'ID',
            'datetime' => 'Datetime',
            'cancelleddatetime' => 'Cancelleddatetime',
            'cancelledreason' => 'Cancelledreason',
            'status' => 'Status',
            'userid' => 'Userid',
        ];
    }

    /**
     * Gets query for [[User]].
     *
     * @return \yii\db\ActiveQuery
     */
    public function getUser()
    {
        return $this->hasOne(User::className(), ['id' => 'userid']);
    }

    /**
     * Gets query for [[HelpItems]].
     *
     * @return \yii\db\ActiveQuery
     */
    public function getHelpItems()
    {
        return $this->hasMany(HelpItem::className(), ['helpid' => 'id']);
    }

    /**
     * Gets query for [[Messages]].
     *
     * @return \yii\db\ActiveQuery
     */
    public function getMessages()
    {
        return $this->hasMany(Message::className(), ['helpid' => 'id']);
    }
}
