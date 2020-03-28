<?php

namespace app\modules\v1\models;

use Yii;

/**
 * This is the model class for table "help_category".
 *
 * @property string $id
 * @property string $name
 *
 * @property HelpHasCategory[] $helpHasCategories
 * @property Help[] $helps
 */
class HelpCategory extends \yii\db\ActiveRecord
{
    /**
     * {@inheritdoc}
     */
    public static function tableName()
    {
        return 'help_category';
    }

    /**
     * {@inheritdoc}
     */
    public function rules()
    {
        return [
            [['id', 'name'], 'required'],
            [['id', 'name'], 'string', 'max' => 64],
            [['id'], 'unique'],
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
        ];
    }

    /**
     * Gets query for [[HelpHasCategories]].
     *
     * @return \yii\db\ActiveQuery
     */
    public function getHelpHasCategories()
    {
        return $this->hasMany(HelpHasCategory::className(), ['helpCategoryId' => 'id']);
    }

    /**
     * Gets query for [[Helps]].
     *
     * @return \yii\db\ActiveQuery
     */
    public function getHelps()
    {
        return $this->hasMany(Help::className(), ['id' => 'helpId'])->viaTable('help_has_category', ['helpCategoryId' => 'id']);
    }
}
