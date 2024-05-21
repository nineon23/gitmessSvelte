<script lang="ts">
	import type { RepositoryPagePayload } from '$lib/pagePayloads/repository.payload';

	export let data: RepositoryPagePayload;
</script>

<div>{data.name}</div>
