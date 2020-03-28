<?php

namespace app\modules\v1\models;

use Yii;

/**
 * This is the model class for table "message".
 *
 * @property int $id
 * @property string $datetime
 * @property string $text
 * @property string $status
 * @property int $helpId
 * @property int $userId
 *
 * @property Help $help
 * @property User $user
 */
class Message extends \yii\db\ActiveRecord
{
    /**
     * {@inheritdoc}
     */
    public static function tableName()
    {
        return 'message';
    }

    /**
     * {@inheritdoc}
     */
    public function rules()
    {
        return [
            [['datetime', 'text', 'status', 'helpId', 'userId'], 'required'],
            [['datetime'], 'safe'],
            [['status'], 'string'],
            [['status'], 'in', 'range' => ['sent', 'read']],
            [['helpId', 'userId'], 'default', 'value' => null],
            [['helpId', 'userId'], 'integer'],
            [['text'], 'string', 'max' => 64],
            [['helpId'], 'exist', 'skipOnError' => true, 'targetClass' => Help::className(), 'targetAttribute' => ['helpId' => 'id']],
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
            'text' => 'Text',
            'status' => 'Status',
            'helpId' => 'Help ID',
            'userId' => 'User ID',
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

    /**
     * Gets query for [[User]].
     *
     * @return \yii\db\ActiveQuery
     */
    public function getUser()
    {
        return $this->hasOne(User::className(), ['id' => 'userId']);
    }
}
