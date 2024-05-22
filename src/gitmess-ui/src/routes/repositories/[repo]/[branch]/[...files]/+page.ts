import type { RepositoryPagePayload, RepositoryPath } from "$lib/pagePayloads/repository.payload";
import type { PageLoad, RouteParams } from "./$types";

function getUrl(params: RouteParams, name: string) {
  if (params.files === '') return `/repositories/${params.repo}/${params.branch}/${name}`;
  return `/repositories/${params.repo}/${params.branch}/${params.files}/${name}`;
}

export const load: PageLoad = ({ params }) => {
  const paths: RepositoryPath[] = params.files
    .split('/')
    .reduce((acc, path) => {
      if (acc.length === 0) {
        const repoPath: RepositoryPath = { url: path, name: path };
        return [repoPath];
      }

      const lastPath = acc[acc.length - 1];
      const newPath: RepositoryPath = { url: `${lastPath.url}/${path}`, name: path };
      return [...acc, newPath];
    }, [] as RepositoryPath[])
    .map(repoPath => ({ ...repoPath, url: `/repositories/${params.repo}/${repoPath.url}` }));

  if (params.files.indexOf('.') > 0) {
    return {
      repo: params.repo,
      branch: params.branch,
      paths,
      type: 'file',
      content: `This is the content of ${params.files}`,
      extension: params.files.substring(params.files.lastIndexOf('.') + 1)
    } as RepositoryPagePayload;
  }

  return {
    repo: params.repo,
    branch: params.branch,
    paths,
    type: 'directory',
    items: [
      {
        type: 'directory',
        name: 'src',
        url: getUrl(params, 'src')
      },
      {
        type: 'file',
        name: 'README.md',
        url: getUrl(params, 'README.md')
      }
    ],
  } as RepositoryPagePayload;
}
