using Application.DTOs.Board;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Services;

    public class BoardService
    {
        private readonly IBoardRepository _boardRepository;

        public BoardService(IBoardRepository boardRepository)
        {
            _boardRepository = boardRepository;
        }

        public async Task<BoardDTO> CreateBoardAsync(CreateBoardDTO dto, string username)
        {
            var board = new Board
            {
                Name = dto.Name,
                Description = dto.Description,
                OwnerId = dto.OwnerId,
                IsArchived = false,
            };

            var createdBoard = await _boardRepository.AddBoardAsync(board, username);

            return new BoardDTO
            {
                Id = createdBoard.Id,
                Name = createdBoard.Name,
                Description = createdBoard.Description,
                OwnerId = createdBoard.OwnerId,
                IsArchived = createdBoard.IsArchived,
            };
        }

        public async Task<BoardDTO?> GetBoardByIdAsync(int id)
        {
            var board = await _boardRepository.GetBoardByIdAsync(id);
            return board == null ? null : new BoardDTO
            {
                Id = board.Id,
                Name = board.Name,
                Description = board.Description,
                OwnerId = board.OwnerId,
                IsArchived = board.IsArchived,
            };
        }

        public async Task<List<BoardDTO>> GetAllBoardsAsync()
        {
            var boards = await _boardRepository.GetAllBoardsAsync();
            return boards.Select(board => new BoardDTO
            {
                Id = board.Id,
                Name = board.Name,
                Description = board.Description,
                OwnerId = board.OwnerId,
                IsArchived = board.IsArchived,
            }).ToList();
        }
        
        public async Task<List<BoardDTO>> GetBoardsAsync(string username)
        {
            var boards = await _boardRepository.GetBoardsAsync(username);
            return boards.Select(board => new BoardDTO
            {
                Id = board.Id,
                Name = board.Name,
                Description = board.Description,
                OwnerId = board.OwnerId,
                IsArchived = board.IsArchived,
            }).ToList();
        }
    }