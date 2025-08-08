<script lang="ts">
  let file: File | null = null;

  async function uploadLap() {
    if (!file) return;

    const formData = new FormData();
    formData.append('file', file);

    await fetch('/api/upload', {
      method: 'POST',
      body: formData,
    });

    alert('Lap uploaded!');
  }
</script>

<h1 class="text-2xl font-bold mb-4">Upload Lap</h1>

<input
  type="file"
  class="mb-4"
  on:change={(e) => {
    const target = e.target as HTMLInputElement | null;
    file = target?.files?.[0] ?? null;
  }}
/>
<button on:click={uploadLap} class="bg-blue-600 text-white px-4 py-2 rounded">
  Upload
</button>
