<script lang="ts">
	import { page } from '$app/stores';
	import GitDirectory from '$lib/components/GitDirectory.svelte';
	import { CodeBlock } from '@skeletonlabs/skeleton';
	import type { GetTreeResponse } from '$lib/models/api.model';
	import { getRepositoryTree } from '$lib/api';

	let response: GetTreeResponse | null = null;

	page.subscribe(async () => {
		response = await getRepositoryTree(
			$page.params.repo,
			$page.params.branch,
			encodeURIComponent($page.params.files)
		);
	});
</script>

<div class="card p-4">
	<header>
		<h3 class="h3">{$page.params.repo}</h3>
		<h4 class="h4">{$page.params.branch}</h4>
	</header>

	{#if response}
		<article class="mt-4">
			{#if response.tree}
				<GitDirectory url={$page.url.toString()} tree={response.tree} />
			{/if}
			{#if response.content}
				<CodeBlock language="ts" code={response.content}></CodeBlock>
			{/if}
		</article>
	{/if}
</div>
