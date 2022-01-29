<template>
    <v-dialog
      v-model="showModal"
      width="290"
      persistent
    >
  <v-card>
        <v-card-title class="text-h5">
          input parameter(s)
        </v-card-title>
        <v-form ref="form" lazy-validation @submit.prevent>
            <v-container>
                <div v-for="(item, index) in procParameters" :key="index">
                      <v-text-field
                        autofocus
                        v-model="item.val"
                        :label="item.colname"
                        :rules="[() => !!item.val || 'This field is required']"
                        required
                        v-on:keyup.13="execute()"  
                      ></v-text-field> 
                </div>
            </v-container>
        </v-form>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            color="red darken-1"
            text
            @click="close()"
          >
            close
          </v-btn>
             <v-btn
            color="green darken-1"
            text
            @click="execute()"
          >
            execute
          </v-btn>
           </v-card-actions>
  </v-card>
    </v-dialog>
</template>


<script>
export default {
    props: {
        showModal: {
        type: Boolean,
        default: false,
        },
        procParameters: {
            type: Array,
            default: null,
        },
    },
    methods: {
        execute() {
            if (!this.$refs.form.validate()) return;

            this.$emit("childToParentSuccess");
            this.$emit("update:showModal", false); 
        },
        close() {
            this.$emit("update:showModal", false);
        },
    },
}
</script>