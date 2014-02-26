$(function () {
    "use strict";

    var gameLobby = $.connection.gameLobby
      , viewModel = new ViewModel()
      , loading = true;

    gameLobby.client.addGame = function (id, host, description) {
        gameLobby.server.getGames();
    };

    gameLobby.client.removeGame = function (id) {
        viewModel.games.remove(function (item) {
            return item.gameId === id;
        });
    };

    gameLobby.client.updateGames = function (games) {
        viewModel.games([]);
        for (var i = 0; i < games.length; i++) {
            var newGame = Game(games[i].gameId, games[i].host, games[i].description);
            viewModel.games.push(newGame);
        }

        if (loading) {
            $('#game-lobby-loading-image').parent().hide();
            $('#games').show();

            loading = false;
        }
    };

    function Game(gameId, host, description) {
        return {
            gameId: gameId,
            host: host,
            description: description,
            link: 'JoinGame/' + gameId
        };
    }

    function ViewModel() {
        var self = this;

        self.games = ko.observableArray([]);
    }

    function initHub() {
        $.connection.hub.start().done(function () {
            gameLobby.server.getGames();
        });
    }

    $(document).ready(function () {
        ko.applyBindings(viewModel);

        initHub();

        $('#force-refresh').click(function () {
            gameLobby.server.getGames();
        });

        $('#btn-create').click(function () {
            $('#create-game').toggleClass("hidden");
        });
    });

});