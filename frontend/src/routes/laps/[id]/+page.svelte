<script lang="ts">
  import LapMap from '$lib/components/LapMap.svelte';

  export let data;

</script>

<h1 class="text-2xl font-bold">Lap: {data.id}</h1>
<LapMap></LapMap>   
