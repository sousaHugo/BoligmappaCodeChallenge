using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SecondApplication.Application.Contracts.Repositories;
using SecondApplication.Application.Dtos;

namespace SecondApplication.Application.Features.GetAllPostInfo
{
    public class GetAllPostInfoRequestHandler : IRequestHandler<GetAllPostInfoRequest, IEnumerable<PostInfoDto>>
    {
        private readonly IPostInfoRepository _postInfoRepository;
        private readonly ILogger<GetAllPostInfoRequestHandler> _handlerLogger;
        private readonly IMapper _mapper;

        public GetAllPostInfoRequestHandler(IPostInfoRepository PostInfoRepository, 
            ILogger<GetAllPostInfoRequestHandler> HandlerLogger,
            IMapper Mapper)
        {
            _postInfoRepository = PostInfoRepository ?? throw new ArgumentNullException(nameof(PostInfoRepository));
            _handlerLogger = HandlerLogger ?? throw new ArgumentNullException(nameof(HandlerLogger));
            _mapper = Mapper ?? throw new ArgumentNullException(nameof(Mapper));
        }

        /// <summary>
        /// This method is responsible for getting the Post Information stored in the the database.
        /// </summary>
        /// <param name="GetAllPostInfoRequest">The request doesnt have any parameter field.</param>
        /// <returns>IEnumerable<PostInfoDto> - List of Posts stored in the database.</returns>
        public async Task<IEnumerable<PostInfoDto>> Handle(GetAllPostInfoRequest Request, 
            CancellationToken CancellationToken)
        {
            _handlerLogger.LogInformation("GetAllPostInfoRequestHandler Handle has started.");

            var postInfoEfList = await _postInfoRepository.GetAllAsync();

            if (postInfoEfList is null)
            {
                _handlerLogger.LogInformation("No Posts found.");
                return new List<PostInfoDto>();
            }

            var postInfoDtoList = postInfoEfList.Select(a => _mapper.Map<PostInfoDto>(a));

            _handlerLogger.LogInformation("GetAllPostInfoRequestHandler Handle has ended.");

            return postInfoDtoList;
        }
    }
}
