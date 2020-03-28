<?php

namespace app\modules\v1\channels;

use Ratchet\ConnectionInterface;
use Ratchet\WebSocket\MessageComponentInterface;

class ChatMessage implements MessageComponentInterface
{
    protected $clients;

    public function __construct() {
        $this->clients = new \SplObjectStorage;
    }

    /**
     * @inheritDoc
     */
    function onOpen(ConnectionInterface $conn)
    {
        $this->clients->attach($conn);
    }

    /**
     * @inheritDoc
     */
    function onClose(ConnectionInterface $conn)
    {
        $this->clients->detach($conn);
    }

    /**
     * @inheritDoc
     */
    function onError(ConnectionInterface $conn, \Exception $e)
    {
        $conn->close();
    }

    /**
     * @inheritDoc
     */
    function onMessage(ConnectionInterface $from, $message)
    {
        foreach ($this->clients as $client) {
            if ($from != $client) {
                $client->send($message);
            }
        }
    }
}
