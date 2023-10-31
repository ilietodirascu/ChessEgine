using ChessEngine.BoardRepresentation;
using ChessEngine.Enums;
using ChessEngine.Extensions;
using ChessEngine.Game;
using ChessEngine.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChessEngine.Utility
{
    public static class FenNotation
    {
        private static string _blackPieces = "rnbqkp";
        private static string _witePieces = "RNBQKP";
        public static string Files = "abcdefgh";
        public static readonly string StartPosFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
        public static FenNotationResponse? GetNotation(string fenString)
        {
            fenString =fenString.Trim();
            string pattern = @"^(?# Piece Placement Section)(?<PiecePlacement>((?<RankItem>[pnbrqkPNBRQK1-8]{1,8})\/?){8})(?# Section Separator)\s+(?# Side To Move Section)(?<SideToMove>b|w)(?# Section Separator)\s+(?# Castling Ability)(?<Castling>-|K?Q?k?q)(?# Section Separator)\s+(?# En passant)(?<EnPassant>-|[a-h][3-6])(?# Section Separator)\s+(?# Half Move Clock)(?<HalfMoveClock>\d+)(?# Section Separator)\s+(?# Full Move Number)(?<FullMoveNumber>\d+)\s*$";
            Match match = Regex.Match(fenString, pattern);

            if (match.Success)
            {
                string piecePlacement = match.Groups["PiecePlacement"].Value;
                string activeColor = match.Groups["SideToMove"].Value;
                string castlingRights = match.Groups["Castling"].Value;
                string enPassantSquare = match.Groups["EnPassant"].Value;
                string halfMoveClock = match.Groups["HalfMoveClock"].Value;
                string fullMoveNumber = match.Groups["FullMoveNumber"].Value;

                return new FenNotationResponse(piecePlacement, activeColor, castlingRights, enPassantSquare, halfMoveClock, fullMoveNumber);
            }

            return null;
        }
        public static void AddMoveNotation(this Move move,bool isPromotion = false )
        {
            var startSquare = move.StartSquare;
            var endSquare = move.EndSquare;
            var result = $"{Files[startSquare.File]}{startSquare.Rank + 1}{Files[endSquare.File]}{endSquare.Rank + 1}";
            move.Notation = isPromotion ? result + move.EndSquarePiece!.ToString()!.ToLower() : result;
        }
        
        public static (PieceValue,Color) GetPieceByChar(char pieceNotation)
        {
            var color = _witePieces.Contains(pieceNotation) ? Color.White : Color.Black;
            pieceNotation = char.ToLower(pieceNotation);
            return pieceNotation switch
            {
                'p' => (PieceValue.Pawn, color),
                'r' => (PieceValue.Rook, color),
                'n' => (PieceValue.Knight, color),
                'b' => (PieceValue.Bishop, color),
                'q' => (PieceValue.Queen, color),
                'k' => (PieceValue.King, color),
                _ => throw new Exception("Invalid Piece")
            };
        }
    }
    
    public class FenNotationResponse
    {
        public string PiecePlacement { get; set; }
        public string ActiveColor { get; set; }
        public string CastlingRights { get; set; }
        public string EnpassantSquare { get; set; }
        public string HalfMoveClock { get; set; }
        public string FullMoveNumber { get; set; }

        public FenNotationResponse(string piecePlacement, string activeColor, string castlingRights, string enpassantSquare, string halfMoveClock, string fullMoveNumber)
        {
            PiecePlacement = piecePlacement;
            ActiveColor = activeColor;
            CastlingRights = castlingRights;
            EnpassantSquare = enpassantSquare;
            HalfMoveClock = halfMoveClock;
            FullMoveNumber = fullMoveNumber;
        }
    }
}
