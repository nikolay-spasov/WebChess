$(function () {
    "use strict";

    var chess = $.connection.chess
      , clicked = false
      , sourceClick
      , move = ""
      , gameId
      , loading = true
      , isWhite = false
      , viewModel = new ViewModel();

    function ViewModel() {
        var self = this;

        self.fields = ko.observableArray([]);

        var selected;
        self.movePiece = function (index, data, event) {
            var piece = viewModel.fields()[index];
            if (clicked === true) {
                if (!isMyPiece(piece)) {
                    viewModel.fields()[index] = sourceClick;
                    move += event.target.parentNode.id.toUpperCase();
                    chess.server.playMove(gameId, move);
                    if (selected) {
                        $(selected).removeClass('selected-piece');
                        selected = null;
                    }
                    clicked = false;
                }
            } else {
                if (isMyPiece(piece)) {
                    sourceClick = piece;
                    viewModel.fields()[index] = 'E';
                    selected = $(event.target).parent();
                    if (viewModel.fields[index] != 'E') 
                        $(selected).addClass('selected-piece');
                    move = event.target.parentNode.id.toUpperCase() + ";";
                    clicked = true;
                }
            }
        }
    }

    chess.client.updateViewModel = function (board) {
        viewModel.fields([]);
        for (var i = 0; i < 64; i++) viewModel.fields.push(board[i]);

        if (loading == true) {
            $('#loading').hide();
            $('#game-board').show();
            $('#chat').show();
            loading = false;
        }
    }

    chess.client.addChatMessage = function (msg) {
        $('#all-messages').append('<li>' + msg + '</li>');
    }

    function initHub() {
        $.connection.hub.start().done(function () {
            chess.server.sendGameId(gameId);
            chess.server.getBoard(gameId);
        });
    }

    function isMyPiece(piece) {
        if (piece === 'E') return false;

        if (isWhite && piece < 'a' && piece > 'A') return true; // Check if piece is in upper case
        else if ((!isWhite) && piece > 'a') return true; // Check if piece is in lower case
    }

    $(document).ready(function () {
        gameId = $('#GameId').val();
        isWhite = $('#IsWhite').val() === 'True';
        ko.applyBindings(viewModel);

        initHub();

        $('#btn-flip').click(function () {
            $("#game-board").toggleClass("flipped-board");
        });

        $('#send-msg').click(sendMessage);
        $('#new-message').keydown(function (eventInfo) {
            if (eventInfo.keyCode === 13) {
                sendMessage();
            }
        });
    });

    function sendMessage() {
        var value = $('#new-message').val();
        $('#new-message').val('');
        chess.server.sendChatMessage(gameId, value);
    }

});