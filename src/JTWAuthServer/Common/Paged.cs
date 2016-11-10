using System.Collections.Generic;

namespace JTWAuthServer.Common {
    public class Paged<T> : List<T> {
        public Paged(IList<T> data, int totalCount) {
            TotalCount = totalCount;
            AddRange(data);
        }

        public int TotalCount {
            get;
        }
    }
}
