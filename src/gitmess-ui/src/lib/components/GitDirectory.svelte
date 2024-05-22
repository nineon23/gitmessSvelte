<script lang="ts">
	import type { GitItem } from '$lib/models/git-item.model';

	export let items: GitItem[];
</script>

<div class="table-container">
	<table class="table table-hover">
		<thead>
			<tr>
				<th></th>
				<th>Name</th>
			</tr>
		</thead>
		<tbody>
			{#each items as row}
				<tr>
					<td>{row.type === 'directory' ? '📁' : '📄'}</td>
					<td><a href={row.url}>{row.name}</a></td>
				</tr>
			{/each}
		</tbody>
	</table>
</div>
