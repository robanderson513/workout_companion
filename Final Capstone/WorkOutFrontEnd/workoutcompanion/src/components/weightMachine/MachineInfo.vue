<template>
  <div> 
      <h1 id="machine-title"> {{machine.name}}</h1>
      <div class="machine_container">
            <div id="machine-image" >
                <img :src="machine.imageURL"/>
            </div>
            <div id="description">
                <p> {{machine.description}}</p>
            </div>
      </div>
      <div id="video">
        <div id="video-link">
            <b-embed            
                type="iframe"
                aspect="16by9"
                :src="URL"
            ></b-embed>
        </div>
      </div>
     
  </div>
</template>

<script>
export default {
  props: {
    machine: Object
  },
  computed: {
    URL: function() {
      return this.machine.videoLink.replace("watch?v=", "embed/");
    }
  }
};
</script>

<style scoped>
.machine_container{
    display: flex;
    width: 100%;
    padding-bottom: 50px;
}
#machine-title {
  display: flex;
  justify-content: center;
  color: rgb(61, 56, 56);
  padding: 15px 0px;
  font-family: Impact, Haettenschweiler, "Arial Narrow Bold", sans-serif;
}
#description {
  display: flex;
  justify-content: center;
  padding: 20px;
}
#description > p {
  width: 80%;
}
#machine-image {
    display: flex;
    justify-content: center;
    width: 50%;
}
img{    
    width: 50%;
    border-radius: 5px;
    border: 1px solid lightgray
}
#video {
  display: flex;
  justify-content: center;
  padding-bottom: 50px;
}
#video-link {
  width: 50%;
}
/*mobile size */
@media screen and (max-width: 600px) {
   .machine_container{
        display: block;
    }
    #machine-image {
        width: 100%;
    }
    #video-link {
        width: 90%;
        padding-bottom: 30px;
    }
}
</style>
