using InflationDataServer.Persistence;
using InflationDataServer.Models;

namespace InflationDataServer.Services
{
    public class ChartsService
    {
        public async Task<ChartsData> readChartsData(DateTime? startDate, DateTime? endDate)
        {
            PostgreSQLUnitOfWork unitOfWork = new PostgreSQLUnitOfWork(DatabaseInformation.GetDbInfo());
            ChartsData chartsData = new ChartsData();
            InflationService inflationService = new InflationService();
            List<Inflation> inflations = await inflationService.readInflation(startDate, endDate);

            // Start both tasks concurrently
            Task<BarChartData> barChartDataTask = Task.Run(() => setupBarChartData(inflations));
            Task<LineChartData> lineChartDataTask = Task.Run(() => setupLineChartData(inflations));
            await Task.WhenAll(barChartDataTask, lineChartDataTask);

            // Retrieve the results
            BarChartData barChartData = barChartDataTask.Result;
            LineChartData lineChartData = lineChartDataTask.Result;
            chartsData.barChartData= barChartData;
            chartsData.lineChartData= lineChartData;
            return chartsData;
        }

        public BarChartData setupBarChartData(List<Inflation> inflations)
        {
            BarChartData barChartData = new BarChartData();
            float variations = 1;

            for (int i = inflations.Count - 1; i >= 0; i--)
            {
                Inflation inflation = inflations[i];
                Console.WriteLine("inflation.value: " + inflation.value);
                float value = 1 + (inflation.value / 100);
                Console.WriteLine("value: " + value.ToString());
                variations *= value;
                Console.WriteLine("variations: " + variations);
                float cumulativeInflation = (float)Math.Round((variations - 1)*100, 2);
                Console.WriteLine("cumulativeinflation: " + cumulativeInflation);
                barChartData.data.Add(cumulativeInflation);
                barChartData.labels.Add(inflation.date.ToString("MMM-yyyy"));
            }
            return barChartData;
        }

        public LineChartData setupLineChartData(List<Inflation> inflations)
        {
            LineChartData lineChartData = new LineChartData();

            for (int i = inflations.Count - 1; i >= 0; i--)
            {
                Inflation inflation = inflations[i];
                LineChartPoint lineChartPoint = new LineChartPoint();
                lineChartPoint.time = inflation.date.ToShortDateString();
                lineChartPoint.value = inflation.value;

                lineChartData.data.Add(lineChartPoint);
            }
            return lineChartData;
        }
    }
}
