<?php

namespace app\modules\v1\models;

use yii\web\IdentityInterface;

/**
 * This is the model class for table "user".
 *
 * @property int $id
 * @property string $name
 * @property float $lat
 * @property float $lng
 * @property string $address
 * @property bool $isVolunteer
 * @property string $accessToken
 *
 * @property Help[] $helps
 * @property Message[] $messages
 */
class User extends \yii\db\ActiveRecord implements IdentityInterface
{
    public function fields()
    {
        $fields = parent::fields();
        // Remove private fields in operations that aren't related to the login
        if($this->scenario !== 'login')
        {
            unset($fields['accessToken']);
        }
        return $fields;
    }

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
            [['name', 'lat', 'lng', 'address', 'isVolunteer'], 'required'],
            [['lat', 'lng'], 'number'],
            [['isVolunteer'], 'boolean'],
            [['name'], 'string', 'max' => 64],
            [['address'], 'string', 'max' => 128],
            [['accessToken'], 'safe', 'on' => 'login'],
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
            'isVolunteer' => 'Is Volunteer',
            'accessToken' => 'Access Token',
        ];
    }

    /**
     * Gets query for [[Helps]].
     *
     * @return \yii\db\ActiveQuery
     */
    public function getHelps()
    {
        return $this->hasMany(Help::className(), ['userId' => 'id']);
    }

    /**
     * Gets query for [[Messages]].
     *
     * @return \yii\db\ActiveQuery
     */
    public function getMessages()
    {
        return $this->hasMany(Message::className(), ['userId' => 'id']);
    }

    /**
     * @inheritDoc
     */
    public static function findIdentity($id)
    {
        return User::findOne(['id' => $id]);
    }

    /**
     * @inheritDoc
     */
    public static function findIdentityByaccesstoken($token, $type = null)
    {
        return User::findOne(['accessToken' => $token]);
    }

    /**
     * @inheritDoc
     */
    public function getId()
    {
        return $this->id;
    }

    /**
     * @inheritDoc
     */
    public function getAuthKey()
    {
        return $this->accessToken;
    }

    /**
     * @inheritDoc
     */
    public function validateAuthKey($authKey)
    {
        return $this->accessToken === $authKey;
    }

    public function refreshAccessToken()
    {
        $this->accessToken = md5(date('c') . $this->name);
    }
}
