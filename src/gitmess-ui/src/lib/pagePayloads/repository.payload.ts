import type { GitItem } from "$lib/models/git-item.model";

export interface RepositoryPath {
  url: string;
  name: string;
}

export interface RepositoryPagePayload {
  repo: string;
  branch: string;
  type: 'directory' | 'file';
  paths: RepositoryPath[];
  items?: GitItem[];
  content?: string;
  extension?: string;
}
