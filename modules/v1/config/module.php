<?php

use yii\rest\UrlRule;
use yii\web\HtmlResponseFormatter;
use yii\web\JsonParser;
use yii\web\JsonResponseFormatter;
use yii\web\Request;
use yii\web\Response;
use yii\web\UrlManager;

return [
    'id' => 'v1',
    'defaultRoute' => 'general',
    'basePath' => dirname(__DIR__),
    'components' => [
        'request' => [
            'class' => Request::class,
            'parsers' => [
                'application/json' => JsonParser::class,
            ],
        ],
        'response' => [
            'class' => Response::class,
            'format' => Response::FORMAT_JSON,
        ],
    ],
    'params' => [
        'name' => 'Covida API',
        'version' => '1.0.0',
    ],
];
