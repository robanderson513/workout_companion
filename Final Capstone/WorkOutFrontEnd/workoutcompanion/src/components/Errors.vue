<template>
  <div>
    <p v-if="errorType === 'firstName'">Please enter a first name</p>
    <p v-if="errorType === 'lastName'">Please enter a last name</p>
    <p v-if="errorType === 'email'">Please enter an email</p>
    <p v-if="errorType === 'userName'">Please enter a username</p>
    <p v-if="errorType === 'password'">Please enter a password</p>
    <p v-if="errorType === 'confirmPassword'">Passwords do not match</p>
    <p v-if="errorType === 'machineName'">Please enter a machine name</p>
    <p v-if="errorType === 'description'">Please enter a description</p>
    <p v-if="errorType === 'file'">Please upload a file</p>
    <p v-if="errorType === 'number'">Please enter a valid number</p>
    <p v-if="errorType === 'classTime'">Please enter a valid time</p>
    <p v-if="errorType === 'classStartDate'">Please enter a valid start date</p>
    <p v-if="errorType === 'className'">Please enter a valid name</p>
    <p v-if="errorType === 'classColor'">Please enter a valid color</p>
  </div>
</template>

<script>
export default {
  props: {
    type: String
  },
  data() {
    return {
      errorType: this.type
    };
  }
};
</script>

<style scoped>
p {
  color: red;
  font-weight: bold;
}
</style>
