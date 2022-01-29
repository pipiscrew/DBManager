<template>
  <v-card>
    <v-card-title>Reports</v-card-title>
    <v-card-title>
        <v-row>
        <v-col cols="12" sm="6" md="3">
        <v-autocomplete
            v-model="selectedRoutine"
            :items="routines"
            :loading="routineLoading"
            item-text="routine"
            label="Select"
            @change="RunRoutine()"
            single-line
        ></v-autocomplete> </v-col>
      </v-row>
    </v-card-title>
    <!-- 
      ## disable pagination by adding ##
      disable-pagination
      hide-default-footer

        tested with 5k - performance is not good, so use pagination!
        https://stackoverflow.com/a/62652053 
        https://vuetifyjs.com/en/components/data-tables/#external-pagination
    -->
    <v-data-table
      :headers="headers"
      :items="records"
      :loading="loading"      
      fixed-header
      height="60vh"
      class="elevation-1"
      :items-per-page="50"
      :footer-props="{
        showFirstLastPage: true,
        itemsPerPageOptions:[50, 250, -1]
      }"
    >

      <template v-slot:footer>
          <div style="margin: 0px 0px -47px 15px">
                  <v-btn  @click="downloadCSV()" :disabled="records==0">
                      <v-icon>mdi-download</v-icon>
                  </v-btn>
          </div>
      </template>
    </v-data-table>
    <ReportsDetail 
      v-if="showReoportsDetail"
      :showModal.sync="showReoportsDetail"
      :procParameters.sync="dialogReportsItem"
      @childToParentSuccess="dialogReportsItemSuccess"
    />
  </v-card>


</template>

<script>
import general from '@/general';
import ReportsDetail from '@/components/ReportsDetail';

export default {
  components: {ReportsDetail},
  data() {
    return {
      showReoportsDetail: false,
      dialogReportsItem : [ {"colname" : "param1", "val": null}, { "colname" : "param2", "val": null}],
      records: [],
      loading: false,
      headers: [],
      routines: [],
      routineLoading:true,
      selectedRoutine:null,
    }
  },
  mounted() {
    this.FillRoutines();
  },
    methods: {
         RunRoutine(){
            if (this.selectedRoutine){

                this.headers = [];
                this.records = [];

                let rType = this.selectedRoutine.substring(0,3);

                if (rType=="[p]"){
                    this.GetRoutineParameters();
                    return;
                }
                else 
                  this.ExecuteView();
            }            
        },
        ExecuteView(){
            this.loading = true;

            this.ApiFillGridExecuteView().then((data) => {
                this.loading = false;

                if (!data)
                  return;

                this.records = data.data;
            });
        },
        FillRoutines(){
            this.routineLoading = true;

            this.ApiFillRoutines().then((data) => {
                this.routineLoading = false;

                    if (!data)
                        return;

                this.routines = data;
            })
        },
        async ApiFillGridExecuteView() {

            let formData = new FormData();
            formData.append("action", "ExecuteView");
            formData.append("rname", this.selectedRoutine);

            const items = await general.GetData(process.env.VUE_APP_ROOT_API + "api/ReportsAPI.php", formData);

            if (!items || items.error) {
                alert("Error in communication with the Server!");
                return null;
            }
            else if (items.data.length==0){
                return null;   
            }

            ////////// GET COLUMN NAMES for DATATABLE
            var gridColumns = new Array();
                Object.keys(items.data[0]).forEach(element => {
                    gridColumns.push( {'text' : element, 'value' : element })
                });

            this.headers = gridColumns;
            //////////

            return items;
        },        
        async ApiFillRoutines() {
            let formData = new FormData();
            formData.append("action", "FillRoutines");

            const items = await general.GetData(process.env.VUE_APP_ROOT_API + "api/ReportsAPI.php", formData);

            if (!items || items.error) {
                alert("Error in communication with the Server!");
                return null;
            }

            return items;
        },
        GetRoutineParameters(){
            this.routineLoading = true;

            this.ApiGetRoutineParameters().then((data) => {
                    this.routineLoading = false;

                    if (!data)
                        return;

                    if (data == "direct_exec_no_params")
                    {
                        this.ExecuteView();
                        return;
                    }
                    ////////// TRANSFORM ROUTINES PARAMETER for REPORTSDETAILS dialog
                    var procParams = new Array();

                    data.forEach(element => {
                        procParams.push( {'colname' : element.parameter_name, 'val' : null })
                    });
                    //////////

                    this.dialogReportsItem = procParams; 

                    this.showReoportsDetail=true;
            })
        },
        async ApiGetRoutineParameters() {
            let formData = new FormData();
            formData.append("action", "GetRoutineParameters");
            formData.append("rname", this.selectedRoutine);

            const items = await general.GetData(process.env.VUE_APP_ROOT_API + "api/ReportsAPI.php", formData);

            if (!items || items.error) {
                alert("Error in communication with the Server!");
                return null;
            }

            if (items.length==0)
                return "direct_exec_no_params";

            return items;
        },
        dialogReportsItemSuccess(){
           this.loading = true;

            this.ApiFillGridExecuteProcedure().then((data) => {
                this.loading = false;

                if (!data)
                  return;

                this.records = data.data;
            });

        },
        async ApiFillGridExecuteProcedure() {

            // this.headers = [];
            // this.items = [];

            let formData = new FormData();
            formData.append("action", "ExecuteProcedure");
            formData.append("rname", this.selectedRoutine);
            formData.append("rparams", JSON.stringify(this.dialogReportsItem));

            const items = await general.GetData(process.env.VUE_APP_ROOT_API + "api/ReportsAPI.php", formData);

            if (!items || items.error) {
                alert("Error in communication with the Server!");
                return null;
            }
            else if (items.data.length==0){ //records returned
                return null;   
            }

            ////////// GET COLUMN NAMES for DATATABLE
            var gridColumns = new Array();
                Object.keys(items.data[0]).forEach(element => {
                    gridColumns.push( {'text' : element, 'value' : element })
                });

            this.headers = gridColumns;
            //////////

            return items;
        },
        downloadCSV(){
          //https://codingshiksha.com/vue/vue-js-material-example-to-download-txt-and-csv-file-as-attachment-in-browser-using-javascript-full-project-for-beginners/
          
          //prepare header
          let csv = "";
          this.headers.forEach(element => {
            csv += element['text'] + '\t';
          });
          
          csv += '\n';

          this.records.forEach(el => {
              var line = "";
              this.headers.forEach(element => {
                  line += el[element['text']] + '\t';
              });
              line +=  '\n';
              csv += line
            })
            var blob = new Blob([ csv ], { "type" : "csv/plain" });
            let link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = this.selectedRoutine.substring(4) + new Date().toISOString().substr(0, 10) + '.xls';
            link.click();
        }
    },
}

</script>