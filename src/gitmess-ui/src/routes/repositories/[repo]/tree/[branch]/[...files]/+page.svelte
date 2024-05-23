<script lang="ts">
	import { page } from '$app/stores';
	import GitDirectory from '$lib/components/GitDirectory.svelte';
	import {CodeBlock, ListBox, ListBoxItem} from '@skeletonlabs/skeleton';
	import type { GetTreeResponse } from '$lib/models/api.model';
	import { getRepositoryTree } from '$lib/api';
	import { onMount } from 'svelte';
	import { goto } from '$app/navigation';

	let branches = [];
	let selectedBranch = '';

	const getBranches = async () => {
		// Replace this with your actual API call or other method of getting branches
		return ['insalata', 'main', 'master'];
	};

	const handleBranchClick = (branch) => {
		selectedBranch = branch;
		goto(`/repositories/${$page.params.repo}/tree/${branch}`);
	};

	onMount(async () => {
		branches = await getBranches();
		await loadTree();
	});
	let response: GetTreeResponse | null = null;

	const loadTree = async () => {
		response = await getRepositoryTree(
			$page.params.repo,
			$page.params.branch,
			encodeURIComponent($page.params.files)
		);
	};

	page.subscribe(loadTree);


</script>

<div class="card p-4">
	<header>
		<h3 class="h3">{$page.params.repo}</h3>
		<h4 class="h4">{$page.params.branch}</h4>
		<ListBox>
			{#each branches as branch}
				<ListBoxItem bind:group={selectedBranch} name="branch" value={branch} on:click={() => handleBranchClick(branch)}>{branch}</ListBoxItem>
			{/each}
		</ListBox>
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
