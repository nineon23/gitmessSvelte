<script lang="ts">
	import GitDirectory from '$lib/components/GitDirectory.svelte';
	import type { RepositoryPagePayload } from '$lib/pagePayloads/repository.payload';
	import { CodeBlock } from '@skeletonlabs/skeleton';

	export let data: RepositoryPagePayload;
</script>

<div class="card p-4">
	{#if data.paths.length > 0}
		<ol class="breadcrumb">
			<li class="crumb">
				<a class="anchor" href="/">/</a>
			</li>
			<li class="crumb-separator" aria-hidden="true">&rsaquo;</li>
			{#each data.paths as pathSeg, i}
				{#if i < data.paths.length - 1}
					<li class="crumb">
						<a class="anchor" href={pathSeg.url}>{pathSeg.name}</a>
					</li>
					<li class="crumb-separator" aria-hidden="true">&rsaquo;</li>
				{:else}
					<li class="crumb">{pathSeg.name}</li>
				{/if}
			{/each}
		</ol>
	{/if}

	<header>
		<h3 class="h3">{data.repo}</h3>
		<h4 class="h4">{data.branch}</h4>
	</header>

	<article class="mt-4">
		{#if data.type === 'directory'}
			<GitDirectory items={data.items} />
		{/if}
		{#if data.type === 'file'}
			<CodeBlock language={data.extension} code={data.content}></CodeBlock>
		{/if}
	</article>
</div>
