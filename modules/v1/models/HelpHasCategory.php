<?php

namespace app\modules\v1\models;

use Yii;

/**
 * This is the model class for table "help_has_category".
 *
 * @property int $helpId
 * @property string $helpCategoryId
 *
 * @property Help $help
 * @property HelpCategory $helpCategory
 */
class HelpHasCategory extends \yii\db\ActiveRecord
{
    /**
     * {@inheritdoc}
     */
    public static function tableName()
    {
        return 'help_has_category';
    }

    /**
     * {@inheritdoc}
     */
    public function rules()
    {
        return [
            [['helpId', 'helpCategoryId'], 'required'],
            [['helpId'], 'default', 'value' => null],
            [['helpId'], 'integer'],
            [['helpCategoryId'], 'string', 'max' => 64],
            [['helpId', 'helpCategoryId'], 'unique', 'targetAttribute' => ['helpId', 'helpCategoryId']],
            [['helpId'], 'exist', 'skipOnError' => true, 'targetClass' => Help::className(), 'targetAttribute' => ['helpId' => 'id']],
            [['helpCategoryId'], 'exist', 'skipOnError' => true, 'targetClass' => HelpCategory::className(), 'targetAttribute' => ['helpCategoryId' => 'id']],
        ];
    }

    /**
     * {@inheritdoc}
     */
    public function attributeLabels()
    {
        return [
            'helpId' => 'Help ID',
            'helpCategoryId' => 'Help Category ID',
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
     * Gets query for [[HelpCategory]].
     *
     * @return \yii\db\ActiveQuery
     */
    public function getHelpCategory()
    {
        return $this->hasOne(HelpCategory::className(), ['id' => 'helpCategoryId']);
    }
}
