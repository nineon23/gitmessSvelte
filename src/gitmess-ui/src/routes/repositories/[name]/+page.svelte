<script lang="ts">
	import type { GitItem } from '$lib/models/git-item.model';
	import type { RepositoryPagePayload } from '$lib/pagePayloads/repository.payload';
	import GitDirectory from '../../../components/GitDirectory.svelte';

	export let data: RepositoryPagePayload;

	const items: GitItem[] = [
		{ type: 'directory', name: 'src' },
		{ type: 'file', name: 'README.md' }
	];
</script>

<div class="card p-4">
	<header>
		<h3 class="h3">{data.name}</h3>
	</header>
	<article class="mt-4">
		<GitDirectory {items} />
	</article>
</div>
