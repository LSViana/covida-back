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
 * @property int $helpid
 * @property int $userid
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
            [['datetime', 'text', 'status', 'helpid', 'userid'], 'required'],
            [['datetime'], 'safe'],
            [['status'], 'string'],
            [['helpid', 'userid'], 'default', 'value' => null],
            [['helpid', 'userid'], 'integer'],
            [['text'], 'string', 'max' => 64],
            [['helpid'], 'exist', 'skipOnError' => true, 'targetClass' => Help::className(), 'targetAttribute' => ['helpid' => 'id']],
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
            'text' => 'Text',
            'status' => 'Status',
            'helpid' => 'Helpid',
            'userid' => 'Userid',
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

    /**
     * Gets query for [[User]].
     *
     * @return \yii\db\ActiveQuery
     */
    public function getUser()
    {
        return $this->hasOne(User::className(), ['id' => 'userid']);
    }
}
