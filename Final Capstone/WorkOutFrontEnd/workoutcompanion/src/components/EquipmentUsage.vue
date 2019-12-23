<template>
  <div>
    <div class="chart-container">
      <!-- <h2 class="title"> Here's what people are using </h2> -->
       <div class="graph_container">
        <GChart
          id="bar-chart"
          :settings="{packages: ['bar']}"    
          :data="chartData"
          :options="chartOptions"
          :resizeDebounce="500"
          :createChart="(el, google) => new google.charts.Bar(el)"
          @ready="onChartReady"
        />
        </div>
    </div>
  </div>
</template>

<script>
import { apiService } from "@/Services/api.js";
const apiSvc = new apiService();

import { GChart } from "vue-google-charts";

export default {
  data() {
    return {
      chartData: []
    };
  },
  methods: {
    onChartReady(chart, google) {
      this.chartsLib = google;
    },
    pullChartData(data) {
      const chartData = [];
      chartData.push(["", "Average Time (Minutes)"]);
      data.forEach(item => {
        chartData.push([
          item.equipName,
          Math.round(item.sumUses / item.countUses)
        ]);
      });
      return chartData;
    }
  },
  computed: {
    chartOptions() {
      return {
        chart: {
          title: "Machine Usage",
          subtitle: "Average time spent on machines"
        },
        bars: "vertical",
        hAxis: { format: "decimal" },
        height: 400,
        width: 1150,
        colors: ['#355bef'],
        backgroundColor: {fill: '#f5f5f5'}
      }
    }
  },
  components: {
    GChart
  },
  created() {
    apiSvc.machineSummary().then(data => {
      this.chartData = this.pullChartData(data);
    });
  }
};
</script>

<style scoped>
.title {
  width: 35%;
}
.chart-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding-top: 20px;
}
#bar-chart {
  display: flex;
  justify-content: center;
}
.graph_container{
  background-color: white;
  border-radius: 15px;
  border: 1px solid lightgray;
  width:95%;
  padding: 10px;
}
/*mobile size */
@media screen and (max-width: 600px) {

}
</style>
