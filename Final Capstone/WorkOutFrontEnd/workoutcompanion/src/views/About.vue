<template>
  <div class="about">
    <h1 id="title">Your Workout Enthusiasts</h1>
      <div class="people">
        <div class='person-container'>
            <h1 class="name"> Therese Nangast </h1>
            <h2 class="description"> Wizard of the Flexbox </h2>
            <img src='@/assets/Therese.jpg'>
        </div>
        <div class='person-container'>
            <h1 class="name"> Rob Anderson </h1>
            <h2 class="description"> The Great Unifier of the Front and Back </h2>
            <img src='@/assets/Rob.jpg'>
        </div>
        <div class='person-container'>
            <h1 class="name"> Cameron Pleshek </h1>
            <h2 class="description"> Routing All of Our Problems Away </h2>
            <img src='@/assets/Cameron.jpg'>
        </div>
        <div class='person-container'>
            <h1 class="name"> Aaron Ataman </h1>
            <h2 class="description"> The TableMaster </h2>
            <img src='@/assets/Aaron.jpg'>
        </div>
    </div>
  </div>
</template>
<script>
export default {
  data() {
    return {
    };
  }
};
</script>
<style scoped>
#title{
  display: flex;
  justify-content: center;
}
.people{
  display: flex;
  align-items: flex-end;
  justify-content: space-around;
  margin-top: 50px;
}
.person-container{
  display: grid;
  grid-template-columns: 1fr;
  grid-template-areas:  " title "
                         " description "
                         " image ";
  width: 20%;
  height: 400px;
}
.name{
  display: flex;
  justify-content: center;
  font-size: 32px;
  grid-area: title;
  align-self: center;
}
.description{
  font-size: 24px;
  grid-area: description;
}
.person-container > img {
  grid-area: image;
  align-self: flex-end;
  width: 100%;
}
</style>
