using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.services;

namespace WpfClientt.model {
    public class AdConvertable : Ad {

        private IMapper mapper;

        public new string StateId { get; set; }

        public new string TypeId { get; set; }

        public new string ManufacturerId { get; set; }

        public new string ConditionId { get; set; }

        public new string CategoryId { get; set; }

        public AdConvertable(IMapper mapper) {
            this.mapper = mapper;
        }

    }
}
