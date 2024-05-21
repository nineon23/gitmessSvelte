import type { RepositoryPagePayload } from "$lib/pagePayloads/repository.payload";
import type { PageLoad } from "./$types";

export const load: PageLoad = ({ params }) => {
  return {
    name: params.name,
  } as RepositoryPagePayload;
}
