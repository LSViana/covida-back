<?php

namespace app\websocket;

use app\modules\v1\channels\ChatMessage;
use Exception;
use Ratchet\App;
use Ratchet\Server\EchoServer;
use yii\base\BootstrapInterface;

class WebSocketBootstrap implements BootstrapInterface
{
    /**
     * @inheritDoc
     */
    public function bootstrap($app)
    {
        try {
            $pid = pcntl_fork();
            if($pid > 0) {
                // Create WebSocket app
                $app = new App('0.0.0.0', 8082);
                $app->route('/chat', new ChatMessage, array('*'));
                $app->route('/echo', new EchoServer, array('*'));
                // Start WebSocket app
                $app->run();
            } else {
                throw Error('Unable to start WebSocket server');
            }
        } catch(Exception $exception) {
            echo 'Error starting WebSocket';
            var_dump($exception);
        }
    }
}
