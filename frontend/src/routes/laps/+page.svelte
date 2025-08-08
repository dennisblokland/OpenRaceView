<script lang="ts">
    import { onMount } from "svelte";

  type Lap = {
    id: string;
    trackName: string;
    startTime: string;
    duration: string;
  };

  let laps: Lap[] = [];

  onMount(async () => {
    const res = await fetch('/api/laps');
    laps = await res.json();
  });
</script>

<h1 class="text-2xl font-bold mb-4">Your Laps</h1>

<ul>
  {#each laps as lap}
    <li class="mb-2">
      <a href={`/laps/${lap.id}`} class="text-blue-400 hover:underline">
        {lap.trackName} — {lap.startTime}
      </a>
    </li>
  {/each}
</ul>
