<?php

use app\websocket\WebSocketBootstrap;

$params = require __DIR__ . '/params.php';
if(YII_ENV_DEV) {
    $db = require __DIR__ . '/db_development.php';
} else {
    $db = require __DIR__ . '/db_release.php';
}

$config = [
    'id' => 'covida-back-console',
    'basePath' => dirname(__DIR__),
    'bootstrap' => [
        'log',
        'websocket'
    ],
    'controllerNamespace' => 'app\commands',
    'aliases' => [
        '@bower' => '@vendor/bower-asset',
        '@npm'   => '@vendor/npm-asset',
        '@tests' => '@app/tests',
    ],
    'components' => [
        'cache' => [
            'class' => 'yii\caching\FileCache',
        ],
        'websocket' => [
            'class' => WebSocketBootstrap::class,
        ],
        'log' => [
            'targets' => [
                [
                    'class' => 'yii\log\FileTarget',
                    'levels' => ['error', 'warning'],
                ],
            ],
        ],
        'db' => $db,
    ],
    'params' => $params,
    /*
    'controllerMap' => [
        'fixture' => [ // Fixture generation command line.
            'class' => 'yii\faker\FixtureController',
        ],
    ],
    */
];

if (YII_ENV_DEV) {
    // configuration adjustments for 'dev' environment
    $config['bootstrap'][] = 'gii';
    $config['modules']['gii'] = [
        'class' => 'yii\gii\Module',
    ];
}

return $config;
