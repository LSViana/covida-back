<?php

namespace app\modules\v1\models;

use Yii;

/**
 * This is the model class for table "user".
 *
 * @property int $id
 * @property string $name
 * @property float $lat
 * @property float $lng
 * @property string $address
 * @property bool $isvolunteer
 *
 * @property Help[] $helps
 * @property Message[] $messages
 */
class User extends \yii\db\ActiveRecord
{
    /**
     * {@inheritdoc}
     */
    public static function tableName()
    {
        return 'user';
    }

    /**
     * {@inheritdoc}
     */
    public function rules()
    {
        return [
            [['name', 'lat', 'lng', 'address', 'isvolunteer'], 'required'],
            [['lat', 'lng'], 'number'],
            [['isvolunteer'], 'boolean'],
            [['name'], 'string', 'max' => 64],
            [['address'], 'string', 'max' => 128],
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
            'lat' => 'Lat',
            'lng' => 'Lng',
            'address' => 'Address',
            'isvolunteer' => 'Isvolunteer',
        ];
    }

    /**
     * Gets query for [[Helps]].
     *
     * @return \yii\db\ActiveQuery
     */
    public function getHelps()
    {
        return $this->hasMany(Help::className(), ['userid' => 'id']);
    }

    /**
     * Gets query for [[Messages]].
     *
     * @return \yii\db\ActiveQuery
     */
    public function getMessages()
    {
        return $this->hasMany(Message::className(), ['userid' => 'id']);
    }
}
