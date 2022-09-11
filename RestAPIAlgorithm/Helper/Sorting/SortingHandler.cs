using RestAPIAlgorithm.Model;

namespace RestAPIAlgorithm.Helper.Sorting
{
    public static class SortingHandler
    {
        //Sorts data by name
        public static List<CustomerDto> NameSorter(List<CustomerDto> customerDto)
        {
            if (!customerDto.Any())
                return new List<CustomerDto>();
            List<CustomerDto> customerList = new List<CustomerDto>();
            var names = customerDto.Select(x => x.FirstName).ToList();

            string name = string.Empty;
            for (int i = 0; i < names.Count; i++)
            {
                int c = 0;
                for (int j = 1; j < names.Count; j++)
                {
                    if (j > i)
                    {
                        if (names[i][c] != names[j][c])
                        {
                            if (names[i][c] > names[j][c])
                            {
                                name = names[i];
                                names[i] = names[j];
                                names[j] = name;
                            }
                        }
                        else
                        {
                            c = c + 1;
                        }
                    }
                }
                customerList.Add(customerDto.FirstOrDefault(x => x.FirstName == names[i]));
            }
            return customerList;
        }
    }
}
