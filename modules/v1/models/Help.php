<?php

namespace app\modules\v1\models;

use Yii;

/**
 * This is the model class for table "help".
 *
 * @property int $id
 * @property string $datetime
 * @property bool $cancelledDateTime
 * @property string $cancelledReason
 * @property string $status
 * @property int $userId
 *
 * @property User $user
 * @property HelpHasCategory[] $helpHasCategories
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
            [['datetime', 'status', 'userId'], 'required'],
            [['datetime'], 'safe'],
            [['cancelledDateTime'], 'boolean'],
            [['status'], 'string'],
            [['status'], 'in', 'range' => ['awaiting', 'active', 'past', 'cancelled']],
            [['userId'], 'default', 'value' => null],
            [['userId'], 'integer'],
            [['cancelledReason'], 'string', 'max' => 64],
            [['userId'], 'exist', 'skipOnError' => true, 'targetClass' => User::className(), 'targetAttribute' => ['userId' => 'id']],
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
            'cancelledDateTime' => 'Cancelled Date Time',
            'cancelledReason' => 'Cancelled Reason',
            'status' => 'Status',
            'userId' => 'User ID',
        ];
    }

    /**
     * Gets query for [[User]].
     *
     * @return \yii\db\ActiveQuery
     */
    public function getUser()
    {
        return $this->hasOne(User::className(), ['id' => 'userId']);
    }

    /**
     * Gets query for [[HelpHasCategories]].
     *
     * @return \yii\db\ActiveQuery
     */
    public function getHelpHasCategories()
    {
        return $this->hasMany(HelpHasCategory::className(), ['helpId' => 'id']);
    }

    /**
     * Gets query for [[HelpItems]].
     *
     * @return \yii\db\ActiveQuery
     */
    public function getHelpItems()
    {
        return $this->hasMany(HelpItem::className(), ['helpId' => 'id']);
    }

    /**
     * Gets query for [[Messages]].
     *
     * @return \yii\db\ActiveQuery
     */
    public function getMessages()
    {
        return $this->hasMany(Message::className(), ['helpId' => 'id']);
    }
}
