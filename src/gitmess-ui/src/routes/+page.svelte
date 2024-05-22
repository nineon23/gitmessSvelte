<script lang="ts">
	import type { GitRepository } from '$lib/models/api.model';

	const repositories: GitRepository[] = [
		{
			name: 'my-test-repo'
		},
		{
			name: 'my-other-repo'
		}
	];
</script>

<div class="card p-4">
	<header>
		<h3 class="h3">Repos</h3>
	</header>
	<article class="mt-4">
		<nav class="list-nav">
			<ul>
				{#each repositories as repository}
					<li>
						<a href="/repositories/{repository.name}/tree/main">
							<span class="badge">📁</span>
							<span class="flex-auto">{repository.name}</span>
						</a>
					</li>
				{/each}
			</ul>
		</nav>
	</article>
</div>
