
namespace NotificationDemo.Service.Dto
{
    public class ContainerDto<TData>
    {
        public ContainerDto(TData data)
        {
            Data = data;
        }

        public TData Data { get; set; }
    }
}
