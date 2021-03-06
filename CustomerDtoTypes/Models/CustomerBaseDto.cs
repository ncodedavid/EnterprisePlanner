using System.ComponentModel.DataAnnotations;
using ProtoBuf;

namespace CustomersDtoTypes.Models
{
    ///These protoincludes arent optimal, possibly look into combining somehow
    [ProtoContract]
    [ProtoInclude(101, typeof(CustomerDto))]
    [ProtoInclude(102, typeof(CustomerToCreateDto))]
    [ProtoInclude(103, typeof(CustomerToUpdateDto))]
    public abstract class CustomerBaseDto
    {        
        [ProtoMember(2)]
        [Required]
        public string Name { get; set; }
        [ProtoMember(3)]
        [Required]
        public string Address { get; set; }
        [ProtoMember(4)]
        [Required]
        public string Business { get; set; }
    }
}