<?php

use app\modules\v1\ApiModule;
use app\modules\v1\models\User;
use yii\rest\UrlRule;
use yii\web\Request;
use yii\web\Response;
use yii\web\UrlManager;

$params = require __DIR__ . '/params.php';
if(YII_ENV_DEV) {
    $db = require __DIR__ . '/db_development.php';
} else {
    $db = require __DIR__ . '/db_release.php';
}

$config = [
    'id' => 'covida-app',
    'basePath' => dirname(__DIR__),
    'bootstrap' => ['log'],
    'aliases' => [
        '@bower' => '@vendor/bower-asset',
        '@npm'   => '@vendor/npm-asset',
    ],
    'modules' => [
        'v1' => [
            'class' => ApiModule::class,
        ],
    ],
    'components' => [
        'request' => [
            'class' => Request::class,
            'cookieValidationKey' => 'thisiscovida2020',
            'parsers' => [
                'application/json' => yii\web\JsonParser::class,
            ]
        ],
        'response' => [
            'class' => Response::class,
            'format' => Response::FORMAT_JSON,
        ],
        'user' => [
            'identityClass' => User::class,
        ],
        'cache' => [
            'class' => 'yii\caching\FileCache',
        ],
        'log' => [
            'traceLevel' => YII_DEBUG ? 3 : 0,
            'targets' => [
                [
                    'class' => 'yii\log\FileTarget',
                    'levels' => ['error', 'warning'],
                ],
            ],
        ],
        'db' => $db,
        'urlManager' => [
            'class' => UrlManager::class,
            'enablePrettyUrl' => true,
            'showScriptName' => false,
            'rules' => [
                [
                    'class' => UrlRule::class,
                    'controller' => [
                        'v1/authorization' => 'v1/authorization',
                        'v1/general' => 'v1/general',
                        'v1/help' => 'v1/help',
                        'v1/help-category' => 'v1/help-category',
                    ],
                ],
                'GET v1/help-category/<id>' => 'v1/help-category/view'
            ],
        ],
    ],
    'params' => $params,
];

if (YII_ENV_DEV) {
    // configuration adjustments for 'dev' environment
    $config['bootstrap'][] = 'debug';
    $config['modules']['debug'] = [
        'class' => 'yii\debug\Module',
    ];

    $config['bootstrap'][] = 'gii';
    $config['modules']['gii'] = [
        'class' => 'yii\gii\Module',
    ];
}

return $config;
